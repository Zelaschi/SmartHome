import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeMemberService } from '../../../../backend/services/HomeMember/home-member.service';
import HomePermissionStatus from './models/HomePermissionStatus';
import { tap } from 'rxjs/operators';
import HomePermissionsRequest from '../../../../backend/services/HomeMember/models/HomePermissionsRequest';

@Component({
  selector: 'app-home-permissions-checkbox',
  templateUrl: './home-permissions-checkbox.component.html',
  styleUrl: './home-permissions-checkbox.component.css'
})

export class HomePermissionsCheckboxComponent implements OnInit, OnDestroy{
  @Input() value: HomePermissionsRequest = {
    addMemberPermission: false,
    addDevicePermission: false,
    listDevicesPermission: false,
    notificationsPermission: false
  };
  @Input() homeMemberId: string = '';
  @Output() valueChange = new EventEmitter<HomePermissionsRequest>();
  
  onChange(permissionKey: keyof HomePermissionsRequest): void {
    this.value = {
      ...this.value,
      [permissionKey]: !this.value[permissionKey]
    };
    this.valueChange.emit(this.value);
  }

  isChecked(permissionKey: keyof HomePermissionsRequest): boolean {
    return this.value[permissionKey];
  }

  status: HomePermissionStatus = {
    loading: true,
    homePermissions: [],
  }

  private _homePermissionsSubscription: Subscription | null = null;

  constructor(private readonly _homeMemberService: HomeMemberService) {}

  ngOnDestroy(): void {
      this._homePermissionsSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    console.log('Iniciando carga de permisos');
    this.status.loading = true;
    
    this._homePermissionsSubscription = this._homeMemberService.listAllHomePermissions()
      .pipe(
        tap(response => console.log('Respuesta del servidor:', response))
      )
      .subscribe({
        next: (homePermissions) => {
          console.log('Permisos recibidos:', homePermissions);
          this.status.loading = false;
          this.status.homePermissions = homePermissions.map((homePermission) => ({
            value: homePermission.name,
            label: homePermission.name.charAt(0).toUpperCase() + homePermission.name.slice(1).toLowerCase()
          }));
          console.log('Estado final:', this.status);
        },
        error: (error) => {
          console.error('Error loading permissions:', error);
          this.status.loading = false;
          this.status.homePermissions = [];
          this.status.error = 'Error loading permissions';
        },
      });
  }

  saveSelectedPermissions(): void {
    this._homeMemberService.updateHomePermissions(this.value, this.homeMemberId)
      .subscribe({
        next: () => {
          console.log('Permisos guardados exitosamente');
        },
        error: (error) => {
          console.error('Error al guardar permisos:', error);
        }
      });
  }
}


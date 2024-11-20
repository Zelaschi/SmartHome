import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HomeMemberService } from '../../../../backend/services/HomeMember/home-member.service';
import HomePermissionsRequest from '../../../../backend/services/HomeMember/models/HomePermissionsRequest';

@Component({
  selector: 'app-home-permissions-checkbox',
  templateUrl: './home-permissions-checkbox.component.html',
  styleUrl: './home-permissions-checkbox.component.css'
})
export class HomePermissionsCheckboxComponent {
  @Input() value: HomePermissionsRequest = {
    addMemberPermission: false,
    addDevicePermission: false,
    listDevicesPermission: false,
    notificationsPermission: false
  };
  @Input() homeMemberId: string = '';
  @Output() valueChange = new EventEmitter<HomePermissionsRequest>();
  
  status = {
    loading: false,
    error: ''
  };

  constructor(private readonly _homeMemberService: HomeMemberService) {}

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

  saveSelectedPermissions(): void {
    this.status.loading = true;
    console.log('lista permisos', this.value)
    console.log('homeMemberId', this.homeMemberId)
    this._homeMemberService.updateHomePermissions(this.value, this.homeMemberId)
      .subscribe({
        next: () => {
          console.log('Permisos guardados exitosamente');
          this.status.loading = false;
        },
        error: (error) => {
          console.error('Error al guardar permisos:', error);
          this.status.error = 'Error al guardar permisos';
          this.status.loading = false;
        }
      });
  }
}


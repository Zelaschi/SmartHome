import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import HomePermissionStatus from './models/homePermissionStatus';
import { Subscription } from 'rxjs';
import { HomeMemberService } from '../../../../backend/services/HomeMember/home-member.service';

@Component({
  selector: 'app-home-permissions-dropdown',
  templateUrl: './home-permissions-dropdown.component.html',
  styleUrl: './home-permissions-dropdown.component.css'
})
export class HomePermissionsDropdownComponent implements OnInit, OnDestroy{
  @Input() value : string | null = null;
  @Output() valueChange = new EventEmitter<string>();
  
  onChange(selectedValue: string): void {
    this.valueChange.emit(selectedValue);
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
    this._homePermissionsSubscription = this._homeMemberService.listAllHomePermissions().subscribe({
      next: (homePermissions) => {
        this.status = {
          homePermissions: homePermissions.map((homePermission) => ({
            value: homePermission.name,
            label: homePermission.name
          })),
        };
      },
      error: (error) => {
        this.status = { 
          homePermissions : [],
          error,
         };
      },
    });
  }
  

}

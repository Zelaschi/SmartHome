import { Component, Input } from '@angular/core';
import HomePermissionsRequest from '../../../../backend/services/HomeMember/models/HomePermissionsRequest';

@Component({
  selector: 'app-home-member-item',
  templateUrl: './home-member-item.component.html'
})
export class HomeMemberItemComponent {
  @Input() homeMember: any;
  showHomePermissionsCheckbox: boolean = false;

  currentPermissions: HomePermissionsRequest = {
    addMemberPermission: false,
    addDevicePermission: false,
    listDevicesPermission: false,
    notificationsPermission: false
  };

  UpdateHomePermissions(): void {
    this.showHomePermissionsCheckbox = !this.showHomePermissionsCheckbox;
    this.currentPermissions = {
      addMemberPermission: this.homeMember.homePermissions.includes('AddMemberPermission'),
      addDevicePermission: this.homeMember.homePermissions.includes('AddDevicesPermission'),
      listDevicesPermission: this.homeMember.homePermissions.includes('ListDevicesPermission'),
      notificationsPermission: this.homeMember.homePermissions.includes('NotificationsPermission')
    };
  }

  onPermissionsChange(newPermissions: HomePermissionsRequest): void {
    this.currentPermissions = newPermissions;
  }
}

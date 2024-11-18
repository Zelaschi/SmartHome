import { Component, Input } from '@angular/core';
import HomeMemberResponseModel from '../../../../backend/services/Home/models/HomeMemberResponseModel';

@Component({
  selector: 'app-home-member-item',
  templateUrl: './home-member-item.component.html',
  styleUrl: './home-member-item.component.css'
})
export class HomeMemberItemComponent {
  @Input() homeMember: HomeMemberResponseModel | null = null;
  showHomePermissionsCheckbox = false;
  ngOnInit(): void {
    console.log(this.homeMember);
  }

  UpdateHomePermissions() {
    this.showHomePermissionsCheckbox = !this.showHomePermissionsCheckbox;
  }

  onPermissionsChange(newPermissions: string[]) {
    if (this.homeMember) {
      this.homeMember.homePermissions = newPermissions;
    }
  }
}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationItemComponent } from './notification-item/notification-item.component';
import { HomeOwnerRoutingModule } from './home-owner-routing.module';
import { HomesListComponent } from './homes-list/homes-list.component';
import { HomeItemComponent } from './home-item/home-item.component';
import { HomeMemberItemComponent } from './home-member-item/home-member-item.component';
import { HomeMembersListComponent } from './home-members-list/home-members-list.component';
import { DeviceModule } from '../device/device.module';



@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationItemComponent,
    HomesListComponent,
    HomeItemComponent,
    HomeMemberItemComponent,
    HomeMembersListComponent
  ],
  imports: [
    CommonModule,
    HomeOwnerRoutingModule,
    DeviceModule
  ]
})
export class HomeOwnerModule { }

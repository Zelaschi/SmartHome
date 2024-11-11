import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationItemComponent } from './notification-item/notification-item.component';
import { HomeOwnerRoutingModule } from './home-owner-routing.module';



@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationItemComponent
  ],
  imports: [
    CommonModule,
    HomeOwnerRoutingModule
  ]
})
export class HomeOwnerModule { }

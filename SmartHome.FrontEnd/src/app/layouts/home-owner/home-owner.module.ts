import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationItemComponent } from './notification-item/notification-item.component';
import { HomeOwnerRoutingModule } from './home-owner-routing.module';
import { HomesListComponent } from './homes-list/homes-list.component';
import { HomeItemComponent } from './home-item/home-item.component';



@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationItemComponent,
    HomesListComponent,
    HomeItemComponent
  ],
  imports: [
    CommonModule,
    HomeOwnerRoutingModule
  ]
})
export class HomeOwnerModule { }

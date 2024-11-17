import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationItemComponent } from './notification-item/notification-item.component';
import { HomeOwnerRoutingModule } from './home-owner-routing.module';
import { HomesListComponent } from './homes-list/homes-list.component';
import { HomeItemComponent } from './home-item/home-item.component';
import { HomeMemberItemComponent } from './home-member-item/home-member-item.component';
import { HomeMembersListComponent } from './home-members-list/home-members-list.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormComponent } from '../../components/form/form/form.component';
import { AppModule } from '../../app.module';
import { ButtonComponent } from '../../components/button/button.component';
import { RoomFormComponent } from './room-form/room-form.component';
import { DeviceListComponent } from '../../business-components/device-list/device-list.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { PhotosCarouselComponent } from '../../components/photos-carousel/photos-carousel.component';
import { HomeDeviceListComponent } from './home-device-list/home-device-list.component';
import { HomeDeviceItemComponent } from './home-device-item/home-device-item.component';
import { FilterComponent } from '../../components/filter/filter/filter.component';
import { HomeFormComponent } from './home-form/home-form.component';
import { HomeNameFormComponent } from './home-name-form/home-name-form.component';
import { HomeDeviceNameComponent } from './home-device-name/home-device-name.component';
import { RoomListComponent } from './room-list/room-list.component';
import { RoomItemComponent } from './room-item/room-item.component';
import { IndividualHomeComponent } from './individual-home/individual-home.component';
import { HomePermissionsDropdownComponent } from './home-permissions-dropdown/home-permissions-dropdown.component';
import { DropdownComponent } from "../../components/dropdown/dropdown.component";



@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationItemComponent,
    HomesListComponent,
    HomeItemComponent,
    HomeMemberItemComponent,
    HomeMembersListComponent,
    RoomFormComponent,
    HomeItemComponent,
    HomeDeviceListComponent,
    HomeDeviceItemComponent,
    HomeFormComponent,
    HomeNameFormComponent,
    HomeDeviceNameComponent,
    RoomListComponent,
    RoomItemComponent,
    IndividualHomeComponent,
    HomePermissionsDropdownComponent
  ],
  imports: [
    PaginationComponent,
    PhotosCarouselComponent,
    CommonModule,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    HomeOwnerRoutingModule,
    DeviceListComponent,
    FilterComponent,
    DropdownComponent
]
})
export class HomeOwnerModule { }

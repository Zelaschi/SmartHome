import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { HomesListComponent } from './homes-list/homes-list.component';
import { RoomFormComponent } from './room-form/room-form.component';

const routes: Routes = [
  {
    path: "",
    component: HomesListComponent
  },
  {
    path: "/notifications",
    component: NotificationListComponent
  },
  {
    path: '/addRoom',
    component: RoomFormComponent
  },
  {
    path: '/homeDevices',
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeOwnerRoutingModule { }



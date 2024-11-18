import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { HomesListComponent } from './homes-list/homes-list.component';
import { RoomFormComponent } from './room-form/room-form.component';
import { authorizationGuard } from '../../../backend/guards/authorization.guard';
import { HomeDeviceListComponent } from './home-device-list/home-device-list.component';
import { HomeFormComponent } from './home-form/home-form.component';
import { IndividualHomeComponent } from './individual-home/individual-home.component';

const routes: Routes = [
  {
    path: "",
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'List all users homes' },
    component: HomesListComponent
  },
  {
    path: "notifications",
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'List all users notifications' },
    component: NotificationListComponent
  },
  {
    path: 'createHome',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Home related permission' },
    component: HomeFormComponent
  },
  {
    path: 'individualHome/:id',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Home related permission' },
    component: IndividualHomeComponent
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeOwnerRoutingModule { }



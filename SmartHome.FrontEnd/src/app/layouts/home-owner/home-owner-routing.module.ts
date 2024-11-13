import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { HomesListComponent } from './homes-list/homes-list.component';

const routes: Routes = [
  {
    path: "homes",
    component: HomesListComponent
  },
  {
    path: '',
    component: NotificationListComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeOwnerRoutingModule { }



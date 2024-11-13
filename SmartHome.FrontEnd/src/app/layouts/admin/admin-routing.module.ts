import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { UserListComponent } from './user-list/user-list.component';
import { BusinessOwnerFormComponent } from './business-owner-form/business-owner-form.component';
import { BusinessListComponent } from './business-list/business-list.component';

const routes: Routes = [
  {
    path: '',
    component: UserListComponent
  },
  {
    path: 'registerAdmins',
    component: AdminFormComponent
  },
  {
    path: 'businesses',
    component: BusinessListComponent
  },
  {
    path: 'businesses/newBusinessOwner',
    component: BusinessOwnerFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

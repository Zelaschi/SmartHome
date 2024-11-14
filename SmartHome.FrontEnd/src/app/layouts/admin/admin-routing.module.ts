import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { UserListComponent } from './user-list/user-list.component';
import { BusinessOwnerFormComponent } from './business-owner-form/business-owner-form.component';
import { BusinessListComponent } from './business-list/business-list.component';
import { authorizationGuard } from '../../../backend/guards/authorization.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'List all accounts' },
    component: UserListComponent
  },
  {
    path: 'registerAdmins',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create or delete admin account' },
    component: AdminFormComponent
  },
  {
    path: 'businesses',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'List all businesses' },
    component: BusinessListComponent
  },
  {
    path: 'registerBusinessOwner',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create business owner account' },
    component: BusinessOwnerFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessOwnerFormComponent } from '../admin/business-owner-form/business-owner-form.component';
import { DeviceFormComponent } from './device-form/device-form.component';
import { authorizationGuard } from '../../../backend/guards/authorization.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create device' },
    component: DeviceFormComponent
  },
  {
    path: 'registerBusiness',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create business' },
    component: BusinessOwnerFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessOwnerRoutingModule { }

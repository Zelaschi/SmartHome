import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessOwnerFormComponent } from '../admin/business-owner-form/business-owner-form.component';
import { DeviceFormComponent } from './device-form/device-form.component';
import { authorizationGuard } from '../../../backend/guards/authorization.guard';
import { BusinessFormComponent } from './business-form/business-form.component';
import { ValidatorsDropDownComponent } from './validators-drop-down/validators-drop-down.component';
import { ValidatorFormComponent } from './validator-form/validator-form.component';

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
    component: BusinessFormComponent
  },
  {
    path: 'validators',
    component: ValidatorsDropDownComponent
  },
  {
    path: 'updateValidator',
    component: ValidatorFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessOwnerRoutingModule { }

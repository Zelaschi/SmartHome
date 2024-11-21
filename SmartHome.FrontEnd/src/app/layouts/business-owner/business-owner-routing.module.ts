import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeviceFormComponent } from './device-form/device-form.component';
import { authorizationGuard } from '../../../backend/guards/authorization.guard';
import { BusinessFormComponent } from './business-form/business-form.component';
import { ValidatorFormComponent } from './validator-form/validator-form.component';
import { DeviceImporterFormComponent } from './device-importer-form/device-importer-form.component';
import { MyBusinessItemComponent } from './my-business-item/my-business-item.component';

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
    path: 'updateValidator',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create business' },
    component: ValidatorFormComponent
  },
  {
    path: 'importDevices',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create device' },
    component: DeviceImporterFormComponent
  },
  {
    path: 'myBusiness',
    canActivate: [authorizationGuard],
    data: { requiredSystemPermission: 'Create device' },
    component: MyBusinessItemComponent
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessOwnerRoutingModule { }

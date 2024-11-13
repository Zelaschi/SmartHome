import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessOwnerFormComponent } from '../admin/business-owner-form/business-owner-form.component';
import { DeviceFormComponent } from './device-form/device-form.component';

const routes: Routes = [
  {
    path: '',
    component: DeviceFormComponent
  },
  {
    path: 'registerBusiness',
    component: BusinessOwnerFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessOwnerRoutingModule { }

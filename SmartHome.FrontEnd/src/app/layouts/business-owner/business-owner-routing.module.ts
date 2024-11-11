import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessOwnerFormComponent } from './business-owner-form/business-owner-form.component';

const routes: Routes = [
  {
    path: '',
    component: BusinessOwnerFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessOwnerRoutingModule { }

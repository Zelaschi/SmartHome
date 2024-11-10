// src/app/layouts/device/device-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessFormComponent } from './business-form/business-form.component';
import { BusinessListComponent } from './business-list/business-list.component';

const routes: Routes = [
  {
    path: '',
    component: BusinessFormComponent
  },
  {
    path: 'list',
    component: BusinessListComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessRoutingModule { }



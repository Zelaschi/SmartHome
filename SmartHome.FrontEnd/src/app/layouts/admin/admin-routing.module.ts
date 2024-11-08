import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminFormComponent } from './admin-form/admin-form.component';

const routes: Routes = [
  {
    path: '',
    component: AdminFormComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

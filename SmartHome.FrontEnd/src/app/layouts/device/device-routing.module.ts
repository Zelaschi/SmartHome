// src/app/layouts/device/device-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeviceFormComponent } from './device-form/device-form.component';

const routes: Routes = [
  {
    path: '', // Ruta vacía para que "/device" cargue directamente el DeviceFormComponent
    component: DeviceFormComponent
  },
  {
    path: 'register', // Ruta adicional, si deseas "/device/register" también disponible
    component: DeviceFormComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeviceRoutingModule { }



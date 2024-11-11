import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'businesses',
    loadChildren: () =>
      import('./layouts/business/business.module').then(
        (m) => m.BusinessModule
      ),
  },
  {
    path: 'devices',
    loadChildren: () =>
      import('./layouts/device/device.module').then(
        (m) => m.DeviceModule
      ),
  },
  {
    path: 'admins',
    loadChildren: () =>
      import('./layouts/admin/admin.module').then(
        (m) => m.AdminModule
      ),
  },
  {
    path: 'businessOwners',
    loadChildren: () =>
      import('./layouts/business-owner/business-owner.module').then(
        (m) => m.BusinessOwnerModule
      ),
  },
  {
    path: '',
    loadChildren: () =>
      import('./layouts/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

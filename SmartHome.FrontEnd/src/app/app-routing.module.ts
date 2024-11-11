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
    path: 'homeOwners',
    loadChildren: () =>
      import('./layouts/home-owner/home-owner.module').then(
        (m) => m.HomeOwnerModule
      ),
  },
  {
    path: '',
    loadChildren: () =>
      import('./layouts/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  {
    path: 'homes',
    loadChildren: () =>
      import('./layouts/home/home.module').then(
        (m) => m.HomeModule
      ),
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

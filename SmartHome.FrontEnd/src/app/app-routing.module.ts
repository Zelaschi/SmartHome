import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { noAuthGuard } from '../backend/guards/no-auth.guard';
import { authGuard } from '../backend/guards/auth.guard';

const routes: Routes = [
  {
    path: 'landing',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/landing/landing.module').then(
        (m) => m.LandingModule
      ),
  },
  {
    path: 'admins',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admin/admin.module').then(
        (m) => m.AdminModule
      ),
  },
  {
    path: 'homeOwners',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-owner/home-owner.module').then(
        (m) => m.HomeOwnerModule
      ),
  },
  {
    path: 'businessOwners',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/business-owner/business-owner.module').then(
        (m) => m.BusinessOwnerModule
      ),
  },
  {
    path: '',
    canActivate: [noAuthGuard],
    loadChildren: () =>
      import('./layouts/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  {
    path: '**', 
    loadChildren: () =>
      import('./layouts/page-not-found/page-not-found.module').then(
        (m) => m.PageNotFoundModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

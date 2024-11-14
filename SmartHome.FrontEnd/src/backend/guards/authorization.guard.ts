import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authorizationGuard: CanActivateFn = (route, state) => {

  const systemPermissions = localStorage.getItem('systemPermissions');
  const requiredRole = route.data['requiredSystemPermission'];
  if (!systemPermissions?.includes(requiredRole)) {
    const router = inject(Router);
    return router.parseUrl('/landing');
  }
  return true;
};

import { Component } from '@angular/core';
import { AuthService } from '../../../backend/services/Session/auth.service';
import UserLoggedModel from '../../../backend/services/Session/models/UserLoggedModel';
import { CommonModule, NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ButtonComponent } from '../button/button.component';
import { RoleUpdateService } from '../../../backend/services/RoleUpdate/role-update.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  standalone: true,
  imports: [NgIf, CommonModule, RouterLink ]
})
export class NavbarComponent  {
  userLogged : UserLoggedModel | null = null;

  constructor(private authService: AuthService, private roleService: RoleUpdateService) {}


  ngOnInit(){
    this.authService.userLogged.subscribe({
      next: (user) => {
        this.userLogged = user;
      }
    });
  }
  logOut(){
    this.authService.logout();
  }
  addHomeOwnerPermissionsToBusinessOwner(){
    this.roleService.addHomeOwnerPermissionsToBusinessOwner();
  }
  addHomeOwnerPermissionsToAdmin(){
    this.roleService.addHomeOwnerPermissionsToAdmin();
  }
}
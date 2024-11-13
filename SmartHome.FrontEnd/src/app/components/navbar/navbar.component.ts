import { Component } from '@angular/core';
import { AuthService } from '../../../backend/services/Session/auth.service';
import UserLoggedModel from '../../../backend/services/Session/models/UserLoggedModel';
import { CommonModule, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  standalone: true,
  imports: [NgIf, CommonModule, RouterLink]
})
export class NavbarComponent  {
  userLogged : UserLoggedModel | null = null;

  constructor(private authService: AuthService) {}


  ngOnInit(){
    this.authService.userLogged.subscribe({
      next: (user) => {
        this.userLogged = user;
      }
    });
  }
}
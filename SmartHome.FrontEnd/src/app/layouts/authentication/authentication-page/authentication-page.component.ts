import { Component } from '@angular/core';
import { AuthService } from '../../../../backend/services/Session/auth.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styles: ``
})
export class AuthenticationPageComponent {
  isLoading: boolean = false;
  error: string = "";

  constructor(private authService : AuthService, private router : Router) {}
}

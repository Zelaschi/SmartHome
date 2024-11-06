import { Component } from '@angular/core';
import { AuthService } from '../../../backend/repositories/auth.service';
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

  onLogin(form : NgForm) {
    this.error = "";

    if (form.invalid) {
      return;
    }

    this.isLoading = true;
    this.authService.login(form.value.email, form.value.password).subscribe(
        () => {
          this.isLoading = false;
          this.router.navigate(['/home']);
        },
        error => {
          let errorMessage = "Ocurrió un error al intentar iniciar sesión";
          if (error.error.message) {
            this.error = error.error.errorMessage;
          }
          this.isLoading = false;
        }
    )
  }
}

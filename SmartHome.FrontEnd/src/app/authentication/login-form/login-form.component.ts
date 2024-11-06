import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../backend/repositories/auth.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent {
  readonly formField: any = {
    email: {
      name: 'email',
      required: 'Email es requerido',
      email: 'Email no es valido',
    },
    password: {
      name: 'password',
      required: 'Contraseña es requerida',
      minlength: 'Contraseña debe tener al menos 6 caracteres',
    },
  };

  readonly loginForm = new FormGroup({
    [this.formField.email.name]: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.password.name]: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  constructor(private readonly _router: Router, private authService : AuthService) {}

  public onSubmit(values: any) {
    this.authService.login(values.email, values.password).subscribe(
      () => {
        this._router.navigate(['/home']);
      },
      error => {
        console.error('Error:', error);
      }
    )
    this._router.navigate(['/home']);
  }

  public goToRegister() {
    this._router.navigate(['/register']);
  }
}

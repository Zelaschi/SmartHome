import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../backend/services/Session/auth.service';
import UserCredentialsModel from '../../../../backend/services/Session/models/UserCredentialsModel';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent {
  readonly formField: any = {
    email: {
      name: "email",
      required: "Email is required",
      email: "Email is not valid",
    },
    password: {
      name: "password",
      required: "Password is required",
      minlength: "The password must be at least 6 characters long",
    },
  };

  readonly loginForm = new FormGroup({
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.password.name]: new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  loginStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _sessionService: AuthService
  ) { }

  public onSubmit(values: UserCredentialsModel) {
    this.loginStatus = { loading: true };

    this._sessionService.login(values).subscribe({
      next: (response) => {
        this.loginStatus = null;

        this._router.navigate(["/landing"]);
      },
      error: (error) => {
        this.loginStatus = null;

        this.loginStatus = { error };
      },
    });
  }
  public goToRegister() {
    this._router.navigate(['/register']);
  }
}

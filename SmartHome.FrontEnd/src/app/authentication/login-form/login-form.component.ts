import { Component } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-login-form",
  templateUrl: "./login-form.component.html",
  styles: ``,
})
export class LoginFormComponent {
  readonly formField: any = {
    email: {
      name: "email",
      required: "Email es requerido",
      email: "Email no es valido",
    },
    password: {
      name: "password",
      required: "Contraseña es requerida",
      minlength: "Contraseña debe tener al menos 6 caracteres",
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

  public onSubmit(values: any) {
    console.log(values);
  }
}

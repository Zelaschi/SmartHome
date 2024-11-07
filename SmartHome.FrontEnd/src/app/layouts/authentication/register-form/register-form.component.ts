import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css'],
})
export class RegisterFormComponent {
  // Definición de campos con mensajes de validación
  readonly formField: any = {
    firstName: {
      name: 'firstName',
      required: 'El nombre es requerido',
    },
    lastName: {
      name: 'lastName',
      required: 'El apellido es requerido',
    },
    email: {
      name: 'email',
      required: 'El email es requerido',
      email: 'El email no es válido',
    },
    password: {
      name: 'password',
      required: 'La contraseña es requerida',
      minlength: 'La contraseña debe tener al menos 6 caracteres',
    },
    imageUrl: {
      name: 'imageUrl',
    },
  };

  // Inicializa el formulario con validaciones para cada campo
  readonly registerForm = new FormGroup({
    [this.formField.firstName.name]: new FormControl('', [
      Validators.required,
    ]),
    [this.formField.lastName.name]: new FormControl('', [
      Validators.required,
    ]),
    [this.formField.email.name]: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.password.name]: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    [this.formField.imageUrl.name]: new FormControl(''), // Campo opcional sin validación
  });

  // Método que maneja el envío del formulario
  public onRegister(values: any) {
    console.log(values);
    // Lógica adicional para manejar el registro
  }
}
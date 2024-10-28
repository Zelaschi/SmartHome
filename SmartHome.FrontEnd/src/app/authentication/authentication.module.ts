import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthenticationPageComponent } from './authentication-page/authentication-page.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormComponent } from '../components/form/form/form.component';
import { FormInputComponent } from '../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../components/form/form-button/form-button.component';
import { RouterModule } from '@angular/router';
import { RegisterFormComponent } from './register-form/register-form.component';

@NgModule({
  declarations: [AuthenticationPageComponent, LoginFormComponent, RegisterFormComponent],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    RouterModule,
  ],
})
export class AuthenticationModule {}
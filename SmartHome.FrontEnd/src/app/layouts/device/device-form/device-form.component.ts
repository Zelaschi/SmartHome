// app/layouts/device/device-form/device-form.component.ts
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-device-form',
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.css'],
})
export class DeviceFormComponent {
  // Definición de campos para el formulario reactivo
  readonly formField: any = {
    name: { name: 'name', required: 'El nombre es requerido' },
    modelNumber: { name: 'modelNumber', required: 'El número de modelo es requerido' },
    description: { name: 'description', required: 'La descripción es requerida' },
    type: { name: 'type', required: 'El tipo de dispositivo es requerido' }, // Nuevo campo de tipo
    photos: { name: 'photos' }
  };

  // Creación del formulario reactivo y sus controles
  readonly deviceForm = new FormGroup({
    [this.formField.name.name]: new FormControl('', [Validators.required]),
    [this.formField.modelNumber.name]: new FormControl('', [Validators.required]),
    [this.formField.description.name]: new FormControl('', [Validators.required]),
    [this.formField.type.name]: new FormControl('', [Validators.required]) // Control para el tipo de dispositivo
  });

  // Método para manejar la carga de fotos
  public onPhotoUpload(event: any) {
    const files = event.target.files;
    console.log('Fotos seleccionadas:', files);
    // Aquí puedes añadir lógica adicional para manejar las fotos
  }

  // Método para manejar el registro del dispositivo
  public onRegisterDevice(values: any) {
    console.log('Datos del dispositivo:', values);
    // Aquí puedes agregar la lógica de registro del dispositivo
  }
}


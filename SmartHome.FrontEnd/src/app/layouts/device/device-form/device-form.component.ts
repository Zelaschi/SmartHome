// app/layouts/device/device-form/device-form.component.ts
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DevicesService } from '../../../../backend/services/Device/devices.service';
import { Router } from '@angular/router';
import DeviceTypesModel from '../../../../backend/services/Device/models/DeviceTypesModel';

@Component({
  selector: 'app-device-form',
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.css'],
})
export class DeviceFormComponent {
  // Definición de campos para el formulario reactivo
  readonly formField: any = {
    name: { 
      name: 'name',
      required: 'The name is required' 
    },
    modelNumber: {
      name: 'modelNumber', 
      required: 'The model number is required' 
    },
    description: { 
      name: 'description', 
      required: 'The description is required' 
    },
    type: { 
      name: 'type', 
      required: 'The type is required' 
    },
    photos: { 
      name: 'photos',
      required: 'At least one photo is required'
    }
  };

  // Creación del formulario reactivo y sus controles
  readonly deviceForm = new FormGroup({
    [this.formField.name.name]: new FormControl('', [
      Validators.required
    ]),
    [this.formField.modelNumber.name]: new FormControl('', [
      Validators.required
    ]),
    [this.formField.description.name]: new FormControl('', [
      Validators.required
    ]),
    [this.formField.type.name]: new FormControl('', [
      Validators.required
    ]) // Control para el tipo de dispositivo
  });
  businessStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  deviceTypes: string[] = [];

  constructor(
    private readonly _router: Router,
    private readonly _devicesService: DevicesService
  ){}

  ngOnInit(){
    this.loadDeviceTypes();
  }

  loadDeviceTypes(){
    this.businessStatus = { loading : true};

    this._devicesService.getDeviceTypes().subscribe({
      next: (response) => {
        this.businessStatus = null;
        this.deviceTypes = response.types;
      },
      error: (error) => {
        this.businessStatus = { error };
      },
    });
  }

  // Método para manejar el registro del dispositivo
  onSubmit(values: any) {
    console.log('Datos del dispositivo:', values);
    // Aquí puedes agregar la lógica de registro del dispositivo
  }
}


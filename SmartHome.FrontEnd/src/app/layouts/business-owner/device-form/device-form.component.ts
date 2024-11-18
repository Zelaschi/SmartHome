// app/layouts/device/device-form/device-form.component.ts
import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { DevicesService } from '../../../../backend/services/Device/devices.service';
import { Router } from '@angular/router';
import { DeviceTypeDropdownComponent } from '../../../business-components/device-type-dropdown/device-type-dropdown.component';
import DeviceCreationModel from '../../../../backend/services/Device/models/DeviceCreationModel';



@Component({
  selector: 'app-device-form',
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.css'],
})
export class DeviceFormComponent {
  typeAux: string | null = null;

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
    },
    personDetection: {
      name: 'personDetection'
    },
    movementDetection: {
      name: 'movementDetection'
    },
    indoor: {
      name: 'indoor'
    },
    outdoor: {
      name: 'outdoor'
    },
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
    ]),
    [this.formField.photos.name]: new FormArray([]),
    [this.formField.personDetection.name]: new FormControl(false),
    [this.formField.movementDetection.name]: new FormControl(false),
    [this.formField.indoor.name]: new FormControl(false),
    [this.formField.outdoor.name]: new FormControl(false)
  });

  deviceStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _devicesService: DevicesService
  ){}

  onDeviceTypeChange(values: any){
    this.typeAux = values;
    this.deviceForm.patchValue({
      [this.formField.type.name]: values
    });
    console.log(this.typeAux);
  }

  ngOnInit() {
    this.addPhoto();
  }

  
  getPhotoFormField(index: number) {
    const control = this.photos.at(index) as FormControl;
    return {
      name: `${this.formField.photos.name}.${index}`,
      control: control,
      placeholder: 'URL de la foto',
    };
  }

  get photoControls() {
    return this.photos.controls;
  }

  get photos() {
    return this.deviceForm.get(this.formField.photos.name) as FormArray;
  }

  addPhoto(): void {
    const newPhotoControl = new FormControl('');
    this.photos.push(newPhotoControl);
    console.log('Added photo, total:', this.photos.length);
  }
  
  removePhoto(index: number): void {
    if (this.photos.length > 1) {
      this.photos.removeAt(index);
    }
  }

  // Método para manejar el registro del dispositivo
  public onSubmit(values: DeviceCreationModel) {
    console.log('Datos del dispositivo:',values);
    this.deviceStatus = { loading: true };
    switch (this.typeAux) {
      case 'Security Camera':
        this._devicesService.registerSecurityCamera(values).subscribe({
          next: () => {
            this.deviceStatus = null;
            this._router.navigate(['/landing']);
          },
          error: (error) => {
            this.deviceStatus = { error };
          }
        });
        break;
      case 'Inteligent Lamp':
        this._devicesService.registerIntelligentLamp(values).subscribe({
          next: () => {
            this.deviceStatus = null;
            this._router.navigate(['/landing']);
          },
          error: (error) => {
            this.deviceStatus = { error };
          }
        });
        break;
      case 'Window Sensor':
        this._devicesService.registerWindowSensor(values).subscribe({
          next: () => {
            this.deviceStatus = null;
            this._router.navigate(['/landing']);
          },
          error: (error) => {
            this.deviceStatus = { error };
          }
        });
        break;
      case 'Movement Sensor':
        this._devicesService.registerMovementSensor(values).subscribe({
          next: () => {
            this.deviceStatus = null;
            this._router.navigate(['/landing']);
          },
          error: (error) => {
            this.deviceStatus = { error };
          }
        });
        break;
      default:
        console.log('Tipo de dispositivo no reconocido');
        break;
    }
  }
}

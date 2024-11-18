import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import DropdownOption from '../../../components/dropdown/models/DropdownOption';
import { DeviceImporterService } from '../../../../backend/services/ImportDevice/device-importer.service';
import DeviceImportRequestModel from '../../../../backend/services/ImportDevice/models/DeviceImportRequestModel';

@Component({
  selector: 'app-device-importer-form',
  templateUrl: './device-importer-form.component.html',
  styleUrl: './device-importer-form.component.css'
})
export class DeviceImporterFormComponent {
  selectedImporter: string | null = null;
  
  importerStatus!: {
    loading?: true;
    error?: string;
    importers: Array<DropdownOption>;
  };
  
  constructor(
    private readonly _deviceImporterService: DeviceImporterService
  ) {}
  
  readonly formField: any = {
    fileName: {
      name: "fileName",
    },
    dllName:{
      name:'dllName'
    }
  }
  readonly deviceImporterForm = new FormGroup({
    [this.formField.fileName.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.dllName.name]: new FormControl("", [
      Validators.required,
    ])
  });


  ngOnInit()
  {
    this.importerStatus = {
      importers: [
        {value: 'JSON', label: 'JSON'}
      ]
    }
  }

  onImporterChange(value: string): void {
    this.selectedImporter = value;
    this.deviceImporterForm.patchValue({
      [this.formField.dllName.name]: value
    });
  }

  public onSubmit(values : DeviceImportRequestModel) {
    if (this.deviceImporterForm.invalid) {
      return;
    }

    console.log(values);

    this._deviceImporterService.importDevices(values).subscribe({
      next: () => {
        this.deviceImporterForm.reset();
        this.selectedImporter = null;
      },
      error: (error) => {
        console.error(error);
      }
    });
    //this._businessService.addValidatorToBusiness(validatorId).subscribe({
      //next: () => {
        //this.validatorForm.reset();
        //this.selectedValidator = null;
      //},
      //error: (error) => {
        //console.error(error);
      //}
    //});
  }

}

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
    success?: string;
    loading?: boolean;
    error?: string;
    importers: Array<DropdownOption>;
  };
  
  constructor(
    private readonly _deviceImporterService: DeviceImporterService
  ) {}
  
  readonly formField: any = {
    fileName: {
      name: "fileName",
      required: "File Name is required"
    },
    dllName:{
      name:'dllName',
      required: "DLL Name is required"
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
    this.importerStatus.loading = true ;
    if (this.deviceImporterForm.invalid) {
      this.importerStatus.error = "Please fill in all required fields";
      this.importerStatus.loading = false ;
      return;
    }


    this._deviceImporterService.importDevices(values).subscribe({
      next: (result) => {
        this.importerStatus.error = undefined;
        this.deviceImporterForm.reset();
        this.selectedImporter = null;
        this.importerStatus.loading = false ;
        this.importerStatus.success = result + ' Devices imported successfully';
      },
      error: (error) => {
        this.importerStatus.loading = false ;
        this.importerStatus.success = undefined;
        if(error == undefined){
          this.importerStatus.error = '';
        }
        else
        {
          this.importerStatus.error = error;

        }
        console.error(error);
      }
    });
  }

}

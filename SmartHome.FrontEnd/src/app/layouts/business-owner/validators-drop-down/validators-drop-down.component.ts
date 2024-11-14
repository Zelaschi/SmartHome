import { Component, EventEmitter, Input, Output } from '@angular/core';
import ValidatorStatus from './models/validator-status';
import { Subscription } from 'rxjs';
import { BusinessService } from '../../../../backend/services/Business/business.service';

@Component({
  selector: 'app-validators-drop-down',
  templateUrl: './validators-drop-down.component.html',
  styleUrl: './validators-drop-down.component.css'
})
export class ValidatorsDropDownComponent {
  @Input() value : string | null = null;
  @Output() valueChange = new EventEmitter<string>();

  onChange(selectedValue: string): void {
    this.valueChange.emit(selectedValue);
  }

  status: ValidatorStatus = {
    loading: true,
    validators: [],
  }

  private _validatorsSubscription: Subscription | null = null;

  constructor(private readonly _businessService: BusinessService) {}

  ngOnDestroy(): void {
      this._validatorsSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._validatorsSubscription = this._businessService.getValidators().subscribe({
      next: (validators) => {
        this.status = {
          validators: validators.map((validator) => ({
            value: validator.validatorId,
            label: validator.name
          })),
        };
      },
      error: (error) => {
        this.status = { 
          validators : [],
          error,
         };
      },
    });
  }
}

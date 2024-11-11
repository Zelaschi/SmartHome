import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css',
  standalone: true,
  imports: [ReactiveFormsModule],
})
export class FilterComponent {
  @Input({ required: true }) form!: FormGroup;
  @Output() onSubmit = new EventEmitter<any>();

  public onLocalSubmit() {
    this.onSubmit.emit(this.form.value);
  }
}


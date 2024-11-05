import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './form.component.html',
  styles: ``,
})
export class FormComponent {
  @Input({ required: true }) form!: FormGroup;
  @Output() onSubmit = new EventEmitter<any>();

  public onLocalSubmit() {
    const isValid = this.form.valid;

    if (!isValid) {
      this.form.markAllAsTouched();
      return;
    }

    this.onSubmit.emit(this.form.value);
  }
}

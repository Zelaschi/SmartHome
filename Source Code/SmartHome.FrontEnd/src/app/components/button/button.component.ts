import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [],
  templateUrl: './button.component.html',
  styles: ``,
})
export class ButtonComponent {
  @Input({required: true}) title!: string;
  @Input({required: true}) onClick!: () => void;
}

import { Component, Input } from "@angular/core";

@Component({
  selector: "app-form-button",
  standalone: true,
  imports: [],
  templateUrl: "./form-button.component.html",
  styles: ``,
})
export class FormButtonComponent {
  @Input({ required: true }) title!: string;
}


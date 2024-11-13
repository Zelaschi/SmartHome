import { Component, Input } from "@angular/core";
import { InputComponent } from "../../input/input.component";
import { CommonModule } from "@angular/common";


@Component({
  selector: "app-form-button",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./form-button.component.html",
  styles: ``,
})
export class FormButtonComponent {
  @Input({ required: true }) title!: string;
}


import { Component, Input, input } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-home-item',
  templateUrl: './home-item.component.html',
  styleUrl: './home-item.component.css'
})
export class HomeItemComponent {
  constructor(
    private readonly _router: Router,
  ) { }
  @Input() home: HomeCreatedModel | null = null;

  ngOnInit(): void {
    console.log(this.home);
  }

  HomeInfo(homeId: string): void {
    if (homeId && this.home) {
      const navigationExtras: NavigationExtras = {
        state: {
          homeData: this.home
        }
      };
      this._router.navigate(['/homOwners/individualHome', homeId])
        .catch(error => {
          console.error('Error de navegación:', error);
          // Intenta un método alternativo si el primero falla
          this._router.navigateByUrl(`/homeOwners/individualHome/${homeId}`)
            .catch(err => console.error('Error en navegación alternativa:', err));
        });
    } else {
      console.error('homeId inválido:', homeId);
    }
  }
}

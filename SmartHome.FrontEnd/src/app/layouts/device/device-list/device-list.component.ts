// device-list.component.ts
import { Component, Input, OnInit } from '@angular/core';
import DropdownOption from '../../../components/dropdown/models/DropdownOption';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent implements OnInit {
  @Input() homeId: number | undefined;
  devices = []; // Array de dispositivos simulados

  searchForm: FormGroup;
  formField = {
    search: { required: 'El campo de búsqueda es obligatorio' }
  };

  // Opciones de filtro para el DropdownComponent
  filterOptions: DropdownOption[] = [
    { value: 'all', label: 'Todos' },
    { value: 'connected', label: 'Conectados' },
    { value: 'disconnected', label: 'Desconectados' }
  ];
  
  selectedFilter: string = 'all';

  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      search: ['']
    });
  }

  ngOnInit(): void {
    this.loadMockDevices();
  }

  loadMockDevices(): void {
    this.devices = [
      // Dispositivos simulados
    ];
  }

  onSearch() {
    const searchValue = this.searchForm.get('search')?.value;
    // Lógica para filtrar dispositivos usando searchValue
    console.log("Buscando dispositivos con:", searchValue);
  }

  onFilterChange(selectedFilter: string): void {
    this.selectedFilter = selectedFilter;
    // Lógica de filtrado basada en `selectedFilter`
    console.log("Filtrando dispositivos por:", selectedFilter);
  }
}
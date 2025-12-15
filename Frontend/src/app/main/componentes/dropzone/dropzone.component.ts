import { Component } from '@angular/core';
import * as XLSX from 'xlsx';
import { HttpClient } from '@angular/common/http';

interface AdmonSueldo {
  cia: number | null;
  tipotrab: string | null;
  nomina: number | null;
  nombre: string | null;
  puesto: string | null;
  departamento: string | null;
  segmento: string | null;
  fechaingreso: string | null;
  sueldodiario: number | null;
  sueldomensual: number | null;
  nivelnum: number | null;
  nivel: string | null;
  tipotab: string | null;
  antiguedad: number | null;
  vacio: string | null;
  mediatab: number | null;
  pra: number | null;
  ppa: number | null;
  porctab: number | null;
  porcformula: number | null;
  sueldonuevo: number | null;
  vacio2: string | null;
  mediatab2: number | null;
  nvopra: number | null;
  normppa: number | null;
  ppa_aster: number | null;
  tabulador: number | null;
  posicion: string | null;
  sintope: string | null;
  contope: string | null;
}

@Component({
  selector: 'component-dropzone',
  templateUrl: './dropzone.component.html',
  styleUrls: ['./dropzone.component.css']
})
export class DropzoneComponent {
  excelData: any[] = [];
  previewData: any[] = [];
  columns: string[] = [];

  constructor(private http: HttpClient) {}

  onFileSelect(event: any) {
    const file = event.files ? event.files[0] : event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = (e: any) => {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: 'array' });
      const sheet = workbook.Sheets[workbook.SheetNames[0]];

      // Convertir a JSON, manteniendo celdas vacías
      this.excelData = XLSX.utils.sheet_to_json(sheet, { defval: null });

      // Preview (primeras 10 filas)
      this.previewData = this.excelData.slice(0, 10);

      // Columnas dinámicas
      this.columns = Object.keys(this.previewData[0] || {});
    };
    reader.readAsArrayBuffer(file);
  }

  private parseNumber(v: any): number | null {
    if (v === null || v === undefined) return null;
    if (typeof v === 'string') {
      v = v.replace(/\./g, '').replace(/,/g, '.').replace(/%/g, '');
    }
    const n = Number(v);
    return isNaN(n) ? null : n;
  }

  private parsePercent(v: any): number | null {
    const n = this.parseNumber(v);
    return n !== null ? n / 100 : null;
  }

  private parseDate(v: any): string | null {
    if (!v) return null;

    if (typeof v === 'number') {
      return new Date((v - 25569) * 86400 * 1000).toISOString();
    }

    if (typeof v === 'string') {
      const parts = v.split('/');
      if (parts.length === 3) {
        const [dd, mm, yyyy] = parts.map(Number);
        const d = new Date(yyyy, mm - 1, dd);
        return isNaN(d.getTime()) ? null : d.toISOString();
      }
      const d = new Date(v);
      return isNaN(d.getTime()) ? null : d.toISOString();
    }

    return null;
  }

  private mapRow(row: any): AdmonSueldo {
    const map: { [key: string]: string[] } = {
      cia: ['Cia', 'cia'],
      tipotrab: ['TipoTrab', 'Tipo Trab', 'tipotrab'],
      nomina: ['Nomina', 'nomina'],
      nombre: ['Nombre', 'nombre'],
      puesto: ['Puesto', 'puesto'],
      departamento: ['Departamento', 'departamento'],
      segmento: ['Segmento', 'segmento'],
      fechaingreso: ['FechaIngreso', 'Fecha ingreso', 'fechaingreso'],
      sueldodiario: ['SueldoDiario', 'Sueldo diario', 'sueldodiario'],
      sueldomensual: ['SueldoMensual', 'Sueldo mensual', 'sueldomensual'],
      nivelnum: ['NivelNum', 'Nivel num', 'nivelnum'],
      nivel: ['Nivel', 'nivel'],
      tipotab: ['Tipotab', 'Tipo Tab', 'tipotab'],
      antiguedad: ['Antiguedad', 'Antigüedad', 'antiguedad'],
      vacio: ['Vacio', 'vacio'],
      mediatab: ['Mediatab', 'MEDIA TAB', 'mediatab'],
      pra: ['Pra', 'PRA', 'pra'],
      ppa: ['Ppa', 'PPA', 'ppa'],
      porctab: ['Porctab', 'Porc. Tab', 'porctab'],
      porcformula: ['Porcformula', 'Porc. Fórmula', 'porcformula'],
      sueldonuevo: ['Sueldonuevo', 'Sueldo nuevo', 'sueldonuevo'],
      vacio2: ['Vacio2', 'vacio2'],
      mediatab2: ['Mediatab2', 'MEDIA TAB', 'mediatab2'],
      nvopra: ['Nvopra', 'NVO PRA', 'nvopra'],
      normppa: ['Normppa', 'Norm PPA', 'normppa'],
      ppa_aster: ['PpaAster', 'PPA*', 'ppa_aster'],
      tabulador: ['Tabulador', 'tabulador'],
      posicion: ['Posición', 'posicion'],
      sintope: ['Sin Tope', 'sintope'],
      contope: ['Con Tope', 'contope']
    };

    const getValue = (keys: string[]): any => {
      for (const k of keys) {
        if (row[k] !== undefined && row[k] !== '') return row[k];
      }
      return null;
    };

    const result: any = {};
    for (const prop in map) {
      let val = getValue(map[prop]);
      if (['cia','nomina','sueldodiario','sueldomensual','nivelnum','antiguedad','mediatab','pra','ppa','sueldonuevo','mediatab2','nvopra','normppa','ppa_aster','tabulador'].includes(prop)) {
        val = this.parseNumber(val);
      }
      if (['porctab','porcformula'].includes(prop)) {
        val = this.parsePercent(val);
      }
      if (prop === 'fechaingreso') val = this.parseDate(val);
      result[prop] = val;
    }
    return result as AdmonSueldo;
  }

  sendToApi() {
    if (!this.excelData.length) return alert('No hay datos para enviar');

    const payload = this.excelData.map(row => this.mapRow(row));

    console.log('Payload a enviar:', payload);

    this.http.post('http://localhost:5094/api/Admon_Sueldos', payload)
      .subscribe({
        next: res => alert('Datos insertados correctamente'),
        error: err => {
          console.error('ERROR API:', err);
          alert('Error al insertar datos. Revisa consola.');
        }
      });
  }
}

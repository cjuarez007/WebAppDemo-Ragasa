import { Component, inject } from '@angular/core';
import * as XLSX from 'xlsx';
import { AdmonSueldosService } from '../../services/admon-sueldos.service';

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
  porctab: string | null;
  porcformula: string | null;
  sueldonuevo: number | null;
  vacio2: string | null;
  mediatab2: number | null;
  nvopra: number | null;
  normppa: number | null;
  ppa_aster: number | null;
  tabulador: string | null;
  posicion: string | null;
  sintope: string | null;
  contope: string | null;
}

@Component({
  selector: 'component-dropzone',
  templateUrl: './dropzone.component.html',
  styleUrl: './dropzone.component.css'
})
export class DropzoneComponent {

  rawData: any[] = [];        // datos crudos del excel
  previewData: any[] = [];
  columns: string[] = [];

  private admonSueldosService = inject(AdmonSueldosService);

  onFileSelect(event: any) {
    const file: File = event.files?.[0];
    if (!file) return;

    const extension = file.name.split('.').pop()?.toLowerCase();

    if (extension === 'csv') {
      this.readCSV(file);
    } else if (extension === 'xlsx') {
      this.readXLSX(file);
    }
  }

  private processSheet(sheet: XLSX.WorkSheet) {
    this.rawData = XLSX.utils.sheet_to_json(sheet, { defval: null });

    this.columns = this.rawData.length
      ? Object.keys(this.rawData[0])
      : [];

    this.previewData = this.rawData.slice(0, 10);
  }

  readCSV(file: File) {
    const reader = new FileReader();

    reader.onload = (e: any) => {
      const text = e.target.result;
      const workbook = XLSX.read(text, { type: 'string' });
      const sheet = workbook.Sheets[workbook.SheetNames[0]];
      this.processSheet(sheet);
    };

    reader.readAsText(file);
  }

  readXLSX(file: File) {
    const reader = new FileReader();

    reader.onload = (e: any) => {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: 'array' });
      const sheet = workbook.Sheets[workbook.SheetNames[0]];
      this.processSheet(sheet);
    };

    reader.readAsArrayBuffer(file);
  }

  private mapRow(row: any): AdmonSueldo {
    return {
      cia: row.cia ? Number(row.cia) : null,
      tipotrab: row.tipotrab ?? null,
      nomina: row.nomina ? Number(row.nomina) : null,
      nombre: row.nombre ?? null,
      puesto: row.puesto ?? null,
      departamento: row.departamento ?? null,
      segmento: row.segmento ?? null,
      fechaingreso: row.fechaingreso
        ? new Date(row.fechaingreso).toISOString()
        : null,

      sueldodiario: row.sueldodiario ? Number(row.sueldodiario) : null,
      sueldomensual: row.sueldomensual ? Number(row.sueldomensual) : null,
      nivelnum: row.nivelnum ? Number(row.nivelnum) : null,
      nivel: row.nivel ?? null,
      tipotab: row.tipotab ?? null,
      antiguedad: row.antiguedad ? Number(row.antiguedad) : null,
      vacio: row.vacio ?? null,
      mediatab: row.mediatab ? Number(row.mediatab) : null,
      pra: row.pra ? Number(row.pra) : null,
      ppa: row.ppa ? Number(row.ppa) : null,
      porctab: row.porctab ?? null,
      porcformula: row.porcformula ?? null,
      sueldonuevo: row.sueldonuevo ? Number(row.sueldonuevo) : null,
      vacio2: row.vacio2 ?? null,
      mediatab2: row.mediatab2 ? Number(row.mediatab2) : null,
      nvopra: row.nvopra ? Number(row.nvopra) : null,
      normppa: row.normppa ? Number(row.normppa) : null,
      ppa_aster: row.ppa_aster ? Number(row.ppa_aster) : null,
      tabulador: row.tabulador ?? null,
      posicion: row.posicion ?? null,
      sintope: row.sintope ?? null,
      contope: row.contope ?? null
    };
  }

  sendToApi() {
    const payload: AdmonSueldo[] = this.rawData.map(r => this.mapRow(r));

    console.log('Payload final:', payload);

    this.admonSueldosService.addAdmonSueldos(payload)
      .subscribe({
        next: res => console.log('INSERTADOS:', res),
        error: err => console.error('ERROR API:', err)
      });
  }
}

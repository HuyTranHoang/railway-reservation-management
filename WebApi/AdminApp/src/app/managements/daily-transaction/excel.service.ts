import {Injectable} from '@angular/core';
import * as ExcelJS from 'exceljs';

@Injectable({
  providedIn: 'root',
})
export class ExcelService {

  constructor() {
  }

  public exportToExcel(data: any[], fileName: string, headers: string[]): void {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Data');

    worksheet.addRow(headers);

    data.forEach((rowData: any[]) => {
      worksheet.addRow(rowData);
    });

    workbook.xlsx.writeBuffer().then((buffer: ArrayBuffer) => {
      const blob = new Blob([buffer], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});

      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${fileName}.xlsx`);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    });
  }
}

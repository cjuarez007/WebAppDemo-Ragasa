// import * as fs from "fs";

// export class FileUploadService {
//   constructor(private containerSasUrl: string) {    
//   }

//   async uploadPdf(rutaArchivo: string): Promise<void> {
    
//     const archivo= fs.readFileSync(rutaArchivo);
    

//     const response = await fetch(this.containerSasUrl, {
//       method: "PUT",
//       headers: {
//         "x-ms-blob-type": "BlockBlob",
//       },
//       body: archivo,
//     });

//     if (!response.ok) {
//       const error = await response.text();
//       throw new Error(`Error ${response.status}: ${error}`);
//     }
//   }
// }
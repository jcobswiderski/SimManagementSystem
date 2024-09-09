// import React, { useState, useEffect } from 'react';

// const CreateInspection = () => {
//     const [engineers, setEngineers] = useState([]);
//     const [selectedEngineer, setSelectedEngineer] = useState('');

//     useEffect(() => {
//       refreshData();
//     }, []);
  
//     const refreshData = async () => {
//       try {
//         const response = await fetch(`${process.env.REACT_APP_API_URL}/Users`);
//         const data = await response.json();
//         setEngineers(data);
//       } catch (error) {
//         console.error('Error fetching engineers:', error);
//       }
//     };

//     return ( 
//         <div className="inspection">
//             <h2 className="inspection__title">Zaplanuj nową obsługę</h2>
//             <form>
//               <select id="engineer" value={selectedEngineer} onChange={(e) => setSelectedEngineer(e.target.value)}>
//                   <option value="">--Select executor--</option>
//                   {
//                     engineers.map(engineer => (
//                       <option key={engineer.id} value={engineer.id}> {engineer.name} </option>
//                     ))
//                   }
//             </select>
//             </form>
//         </div>
//     );
// }
 
// export default CreateInspection;
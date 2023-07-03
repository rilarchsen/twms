import Breadcrumb from '../components/Breadcrumb';
import ShiftsTable from '../components/ShiftsTable';
import TableThree from '../components/TableThree';
import TableTwo from '../components/TableTwo';

const Tables = () => {
  return (
    <>
      <Breadcrumb pageName="Tables" />

      <div className="flex flex-col gap-10">
        <ShiftsTable />
        <TableTwo />
        <TableThree />
      </div>
    </>
  );
};

export default Tables;

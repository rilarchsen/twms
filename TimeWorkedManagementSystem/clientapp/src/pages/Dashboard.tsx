import CardFour from '../components/CardFour.tsx';
import CardOne from '../components/CardOne.tsx';
import CardThree from '../components/CardThree.tsx';
import CardTwo from '../components/CardTwo.tsx';
import ChartOne from '../components/ChartOne.tsx';
import ChartThree from '../components/ChartThree.tsx';
import ChartTwo from '../components/ChartTwo.tsx';
import ChatCard from '../components/ChatCard.tsx';
import MapOne from '../components/MapOne.tsx';
import ShiftsTable from '../components/ShiftsTable.tsx';

const Dashboard = () => {
  return (
    <>
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
        <CardOne />
        <CardTwo />
        <CardThree />
        <CardFour />
      </div>

      <div className="mt-4 grid grid-cols-12 gap-4 md:mt-6 md:gap-6 2xl:mt-7.5 2xl:gap-7.5">
          <div className="col-span-12 xl:col-span-12">
              <ShiftsTable />
          </div>
        {/*<ChartTwo />*/}
        {/*<ChartOne />*/}
        {/*<ChartThree />*/}
        {/*<MapOne />*/}

        {/*<ChatCard />*/}
      </div>
    </>
  );
};

export default Dashboard;

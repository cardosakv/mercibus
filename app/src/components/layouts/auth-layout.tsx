import logo from '@/assets/images/mercibus.png';

export function AuthLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="bg-muted flex min-h-svh flex-col items-center justify-start p-12">
      <div className="flex w-full max-w-md flex-col gap-6">
        <a
          href="/"
          className="flex items-center gap-2 self-center font-bold text-primary"
        >
          <div className="flex size-6 items-center justify-center rounded-md">
            <img
              src={logo}
              alt="Mercibus Logo"
            />
          </div>
          Mercibus
        </a>
        {children}
      </div>
    </div>
  );
}
